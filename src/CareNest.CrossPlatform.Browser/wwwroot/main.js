import { dotnet } from './_framework/dotnet.js';

if (typeof window === 'undefined') {
  throw new Error('CareNest browser host must run in a browser.');
}

const dotnetRuntime = await dotnet
  .withDiagnosticTracing(false)
  .withApplicationArgumentsFromQuery()
  .create();

const config = dotnetRuntime.getConfig();
await dotnetRuntime.runMain(config.mainAssemblyName, [globalThis.location.href]);
