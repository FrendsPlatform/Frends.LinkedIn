# Changelog

## [2.0.0] - 2026-11-21

### Changed

- **[Breaking Change]** Updated the task to .NET 8
- **[Breaking Change]** Updated the task inputs to use the standard Input, Connection, and Options parameters.

### Added

- Added `Success` and `Error` properties to the result. The task can now return Success = false with error details in Error
  instead of throwing.
- Added `ThrowErrorOnFailure` option (default true) to choose whether failures throw an exception or are returned in the
  result.
- Added `ErrorMessageOnFailure` option to specify a custom error message used when a failure occurs.

### Removed

- Removed `ThrowExceptionOnErrorResponse` from Options. A non-2xx response now always throws an exception (handled
  according to `ThrowErrorOnFailure`).

### Moved

- Moved the following properties from Options to Connection: `LinkedInAPIVersion`, `ConnectionTimeoutSeconds`,
  `FollowRedirects`, `AllowInvalidCertificate`, `AllowInvalidResponseContentTypeCharSet`, `AutomaticCookieHandling`.

## [1.1.0] - 2025-03-18

### Changed

- Update packages:
  Newtonsoft.Json 12.0.3 -> 13.0.3
  System.ComponentModel.Annotations 4.7.0 -> 5.0.0
  System.Runtime.Caching 7.0.0 -> 9.0.3
  coverlet.collector 3.1.2 -> 6.0.4
  Microsoft.NET.Test.Sdk 16.6.1 -> 17.13.0
  nunit 3.12.0 -> 4.3.2
  NUnit3TestAdapter 3.17.0 -> 5.0.0

## [1.0.0] - 2023-10-31

### Added

- Initial implementation
