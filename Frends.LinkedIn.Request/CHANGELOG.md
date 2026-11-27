# Changelog

## [2.0.0] - 2026-11-21
### Changed
- Updated the task to target .NET 8.
- **[Breaking Change]** Moved the following properties from `Options` to the new `Connection` class (new `Connection` parameter tab):
  `LinkedInAPIVersion`, `Token`, `ConnectionTimeoutSeconds`, `FollowRedirects`, `AllowInvalidCertificate`,
  `AllowInvalidResponseContentTypeCharSet`, `AutomaticCookieHandling`.

### Added
- `Options.ThrowErrorOnFailure` (default `true`) - throw an exception when the request fails.
- `Options.ErrorMessageOnFailure` - custom message to include when the request fails.
- `Result.Success` (bool) and `Result.Error` (`Error` object with `Message` and `AdditionalInfo`).

### Removed
- `Options.ThrowExceptionOnErrorResponse` - an exception is now always thrown when the response status code is not 2xx.

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


## [1.0.0] - 2023-12-18
### Changed
- Initial implementation
