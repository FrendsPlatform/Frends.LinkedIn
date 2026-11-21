# Changelog

## [2.0.0] - 2026-11-21
### Changed
- **[Breaking Change]** Task now targets only .NET 8 (net8.0). Support for net6.0, netstandard2.0 and net471 was removed.
- **[Breaking Change]** Task parameters were reorganized: the `Filter` parameter was renamed to `Input`, and the access `Token` was moved from `Options` to a new `Connection` parameter.
- **[Breaking Change]** Replaced `Options.ThrowExceptionOnErrorResponse` with `Options.ThrowErrorOnFailure`, and added `Options.ErrorMessageOnFailure` for overriding the error message. When `ThrowErrorOnFailure` is false, failures are returned as `Result.Success = false` with `Result.Error` instead of an exception.
- **[Breaking Change]** Removed the unused `Options.AllowInvalidResponseContentTypeCharSet` setting.
- Result now includes `Success` and `Error` properties.
- Documentation examples were improved.

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
