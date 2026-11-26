# Changelog

## [2.0.0] - 2026-11-21
### Changed
- Task now targets only .NET 8
- **[Breaking Change]** Task parameters were reorganized into `Input`, `Connection` and `Options`:
  - Renamed: the `Filter` parameter was renamed to `Input` (properties unchanged: `AdAccountId`, `Reference`, `Name`, `Id`, `TypeFilters`, `StatusFilters`).
  - Moved from `Options` to `Connection`: `Token`, `LinkedInAPIVersion`, `ConnectionTimeoutSeconds`, `FollowRedirects`, `AllowInvalidCertificate`, `AutomaticCookieHandling`.
  - Removed: `Options.ThrowExceptionOnErrorResponse` (a response with a non-2xx status code now always results in an exception being raised), `Options.AllowInvalidResponseContentTypeCharSet` (unused).
  - Added: `Options.ThrowErrorOnFailure` (default `true`; when `false`, failures are returned as `Result.Success = false` with `Result.Error` instead of an exception) and `Options.ErrorMessageOnFailure` (overrides the error message).
- Result now includes new `Success` and `Error` properties.

## [1.1.0] - 2025-03-18
### Changed
- Update packages:
  Newtonsoft.Json                    12.0.1         -> 13.0.3
  System.ComponentModel.Annotations  4.7.0          -> 5.0.0
  System.DirectoryServices           7.0.0          -> 9.0.3
  System.Runtime.Caching             7.0.0          -> 9.0.3
  coverlet.collector                 3.1.2          -> 6.0.4
  Microsoft.NET.Test.Sdk             16.6.1         -> 17.13.0
  nunit                              3.12.0         -> 4.3.2
  NUnit3TestAdapter                  3.17.0         -> 5.0.0
  RestSharp                          110.2.0        -> 112.1.0
  StyleCop.Analyzers                 1.2.0-beta.507 -> 1.2.0-beta.556

## [1.0.1] - 2023-10-31
### Added
- Added multitagreting to net471 and netstandard2.0

## [1.0.0] - 2023-10-05
### Changed
- Initial implementation
