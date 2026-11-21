# Changelog

## [2.0.0] - 2026-11-21
### Changed
- **[Breaking Change]** Updated the task input parameter to `Input`, moved the task to .NET 8, and added success and error details to the result.
- Added `ThrowErrorOnFailure` and `ErrorMessageOnFailure` options for execution exceptions; HTTP status-code throwing remains controlled by `ThrowExceptionOnErrorResponse`.

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
