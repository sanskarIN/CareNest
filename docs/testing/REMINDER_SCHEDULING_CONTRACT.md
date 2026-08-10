# Reminder Scheduling Contract

This document records the deterministic, non-clinical scheduling behavior that automated tests protect in CareNest `1.0.0-rc.1`.

## Safety boundary

CareNest schedules reminders only from explicit user-entered schedule data. It does not calculate dosage, infer how often a medicine should be used, reinterpret medicine strength/instruction text, or decide whether a user should take a medicine.

`ScheduleKind.AsNeeded` creates no automatic reminder occurrences.

## Entity ownership boundary

Reminder planning verifies the local ownership graph before materializing any occurrence:

- the supplied schedule must belong to the supplied medicine record;
- the supplied medicine record must belong to the supplied local profile;
- a persisted `ScheduleTime` carrying a `MedicineScheduleId` must belong to the supplied schedule;
- unbound editor `ScheduleTime` values are allowed before persistence and are interpreted only with the explicitly supplied schedule.

An ownership mismatch throws instead of silently creating a reminder under another medicine or local profile.

Archived profiles produce no automatic occurrences even if a caller reaches the planner without the coordinator's normal archive filter.

## Planning window

Reminder materialization uses a half-open UTC window:

- both `fromUtc` and `toUtc` must have `DateTimeKind.Utc`;
- local or unspecified planning-window values are rejected instead of being silently reinterpreted as UTC;
- an occurrence exactly at `fromUtc` is included;
- an occurrence exactly at `toUtc` is excluded.

This allows adjacent rebuild windows to meet at one boundary without duplicating the boundary occurrence and prevents accidental local-clock reinterpretation.

## Stable occurrence identity

Occurrence identity is derived from:

- schedule identifier;
- local scheduled date/time;
- schedule time-zone identifier;
- whether the occurrence is a follow-up.

The resulting key is deterministic. Rebuilding the same schedule/window produces the same occurrence keys. Duplicate user-entered clock times are collapsed by occurrence key rather than producing duplicate notifications.

## Ordering

Returned occurrences are ordered by `ScheduledUtc`, regardless of the order in which clock times were supplied by the caller.

## Daily and custom date ranges

Daily and custom-date-range schedules use only explicit user-selected clock times. Schedule start/end dates and medicine start/end dates are enforced as user-entered boundaries.

No occurrence is created after the applicable user-entered end date.

## Selected weekdays

Only weekdays represented in the seven supported weekday-mask bits are eligible. A selected-weekday schedule with no selected day is rejected during validation. Unsupported bits are rejected rather than silently ignored.

## Cycle schedules

Cycle schedules require explicit positive on-days and off-days. Calendar days are counted from the schedule start date. CareNest does not derive cycle lengths from medicine text or clinical information.

## Every-N-hours schedules

Every-N-hours schedules require:

- one explicit starting clock time;
- one explicit interval from 1 to 168 hours.

Occurrences advance by the explicit elapsed-time interval. They are not converted into a guessed number of doses per day.

## Follow-ups and snooze handling

A follow-up is a separate occurrence at the explicit `FollowUpMinutes` offset. It has its own stable occurrence key and does not change the original occurrence time.

A snooze action is accepted only when the caller supplies an explicit future UTC timestamp. Null, past, local-kind, and unspecified-kind snooze timestamps are rejected before persistence or platform notification scheduling.

Reminder-coordinator rebuild overrides likewise require a UTC `fromUtc` value.

## Medicine, profile, and schedule state

Automatic occurrences are not created when:

- the schedule is disabled;
- the local profile is archived;
- the medicine is paused;
- the medicine is completed;
- the medicine is archived;
- the schedule is as-needed.

## Time zones and daylight-saving transitions

Stored schedule times remain local user intent associated with the schedule's explicit time-zone identifier.

For a local time that does not exist because the clock moves forward, CareNest does not invent a replacement time in the planner. That invalid local occurrence is not materialized.

For a local time that occurs twice because the clock moves backward, CareNest chooses one deterministic occurrence using the greater of the two UTC offsets. Rebuilding the same window therefore produces the same UTC occurrence and occurrence key.

Automated DST matrix coverage exercises representative zones in North America, Europe, and Australia when those zone identifiers are available on the test host.

Time-zone behavior must remain deterministic and must never be presented as guaranteed delivery. Operating-system permissions, battery restrictions, force-stop/shutdown behavior, and platform notification policies can still affect actual notification delivery.

## Deterministic property coverage

The unit suite includes deterministic randomized/property-style recurrence checks. A fixed random seed is used so failures are reproducible. The property checks verify that:

- daily occurrences stay inside arbitrary half-open UTC windows;
- occurrence keys remain unique within a build result;
- results remain chronological;
- cycle patterns match explicit on/off-day arithmetic across a matrix of values;
- every valid selected-weekday mask emits only selected days;
- representative every-N-hours intervals preserve the exact elapsed UTC spacing entered by the user.

These checks do not generate or infer any clinical schedule.

## Automated coverage

The unit and contract suites protect:

- daily multi-time schedules;
- as-needed no-automatic-reminder behavior;
- selected weekdays and weekday-mask validation;
- cycle on/off patterns;
- custom date-range boundaries;
- medicine end-date boundaries;
- archived-profile suppression;
- paused/completed/archived medicine suppression;
- every-N-hours intervals;
- follow-up separation;
- explicit future-UTC snooze validation;
- disabled schedules;
- stable occurrence keys;
- entity ownership boundaries;
- UTC planning-window validation;
- ambiguous DST local times across representative zones;
- invalid spring-forward local times across representative zones;
- half-open planning windows;
- duplicate-time deduplication;
- chronological result ordering;
- deterministic randomized recurrence properties;
- schedule-validation boundaries.
