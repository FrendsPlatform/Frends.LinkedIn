// <copyright file="Input.cs" company="Frends">
// Copyright (c) Frends. All rights reserved.
// </copyright>
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;

namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// Search criteria for LinkedIn ad accounts.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets the reference used to search for accounts.
    /// </summary>
    /// <example>urn:li:organization:123456</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the account name used to search.
    /// </summary>
    /// <example>Frends Marketing</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the account ID used to search.
    /// </summary>
    /// <example>512500029</example>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the LinkedIn account type filters.
    /// </summary>
    /// <example>[{ "AccountType": "BUSINESS" }]</example>
    public TypeFilter[] TypeFilters { get; set; }

    /// <summary>
    /// Gets or sets the LinkedIn account status filters.
    /// </summary>
    /// <example>[{ "AccountStatus": "ACTIVE" }]</example>
    public StatusFilter[] StatusFilters { get; set; }
}
