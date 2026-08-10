# Reminder Scheduling Contract

This document records the deterministic, non-clinical scheduling behavior that automated tests protect in CareNest `1.0.0-rc.1`.

## Safety boundary

CareNest schedules reminders only from explicit user-entered schedule data. It does not calculate dosage, infer how often a medicine should be used, reinterpret medicine strength/instruction text, or decide whether a user should take a medicine.

`ScheduleKind.AsNeeded` creates no automatic reminder occurrences.

## Planning window

Reminder materialization uses a half-open UTC window:

- an occurrence exactly at `fromUtc` is included;
- an occurrence exactly at `toUtc` is excluded.

This allows adjacent rebuild windows to meet at one boundary without duplicating the boundary occurrence.

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

Only weekdays represented in the explicit weekday mask are eligible. A selected-weekday schedule with no selected day is rejected during validation.

## Cycle schedules

Cycle schedules require explicit positive on-days and off-days. Calendar days are counted from the schedule start date. CareNest does not derive cycle lengths from medicine text or clinical information.

## Every-N-hours schedules

Every-N-hours schedules require:

- one explicit starting clock time;
- one explicit interval from 1 to 168 hours.

Occurrences advance by the explicit elapsed-time interval. They are not converted into a guessed number of doses per day.

## Follow-ups

A follow-up is a separate occurrence at the explicit `FollowUpMinutes` offset. It has its own stable occurrence key and does not change the original occurrence time.

## Medicine and schedule state

Automatic occurrences are not created when:

- the schedule is disabled;
- the medicine is paused;
- the medicine is completed;
- the medicine is archived;
- the schedule is as-needed.

## Time zones and daylight-saving transitions

Stored schedule times remain local user intent associated with the schedule's explicit time-zone identifier.

For a local time that does not exist because the clock moves forward, CareNest does not invent a replacement time in the planner. That invalid local occurrence is not materialized.

For a local time that occurs twice because the clock moves backward, CareNest chooses one deterministic occurrence using the greater of the two UTC offsets. Rebuilding the same window therefore produces the same UTC occurrence and occurrence key.

Time-zone behavior must remain deterministic and must never be presented as guaranteed delivery. Operating-system permissions, battery restrictions, force-stop/shutdown behavior, and platform notification policies can still affect actual notification delivery.

## Automated coverage

The unit suite protects:

- daily multi-time schedules;
- as-needed no-automatic-reminder behavior;
- selected weekdays;
- cycle on/off patterns;
- custom date-range boundaries;
- medicine end-date boundaries;
- paused/completed/archived suppression;
- every-N-hours intervals;
- follow-up separation;
- disabled schedules;
- stable occurrence keys;
- ambiguous DST local times;
- invalid spring-forward local times;
- half-open planning windows;
- duplicate-time deduplication;
- chronological result ordering;
- schedule-validation boundaries.
