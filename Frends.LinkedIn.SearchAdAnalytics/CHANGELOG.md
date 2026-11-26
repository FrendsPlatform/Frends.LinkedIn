# Changelog

## [2.0.0] - 2026-11-21
### Changed
- **[Breaking Change]** Updated the task input parameter to `Input`
- Replaced the `ThrowExceptionOnErrorResponse` option with standardized error options.
- Updated the task to .NET 8
- Moved the connection-related properties `LinkedInAPIVersion`, `Token`, `ConnectionTimeoutSeconds`, `FollowRedirects`, `AllowInvalidCertificate`, `AllowInvalidResponseContentTypeCharSet` and `AutomaticCookieHandling` from `Options` to the new `Connection` class. The task now takes `Input`, `Connection` and `Options` parameters.
- Removed the `ThrowExceptionOnErrorResponse` option. The task now always treats a non-2xx response as a failure.

### Added

- Added the `Connection` class that holds the connection properties listed above.
- Added `Success` and `Error` properties to the result. The task can now return Success = false with error details in Error instead of throwing.
- Added `ThrowErrorOnFailure` option (default true) to choose whether failures (including non-2xx responses) throw an exception or are returned in the result.
- Added `ErrorMessageOnFailure` option to specify a custom error message used when a failure occurs.

## [1.1.0] - 2025-03-18
### Changed
- Update packages:
  Newtonsoft.Json                    12.0.3 -> 13.0.3
  System.ComponentModel.Annotations  4.7.0  -> 5.0.0
  System.Runtime.Caching             7.0.0  -> 9.0.3
  coverlet.collector                 3.1.2  -> 6.0.4
  Microsoft.NET.Test.Sdk             16.6.1 -> 17.13.0
  nunit                              3.12.0 -> 4.3.2
  NUnit3TestAdapter                  3.17.0 -> 5.0.0

## [1.0.0] - 2023-11-02
### Added
- Initial implementation
