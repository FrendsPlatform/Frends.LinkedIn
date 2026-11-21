// <copyright file="AccountStatus.cs" company="Frends">
// Copyright (c) Frends. All rights reserved.
// </copyright>
namespace Frends.LinkedIn.SearchAdAccounts.Definitions;

/// <summary>
/// LinkedIn advertising account statuses.
/// </summary>
public enum AccountStatus
{
    /// <summary>Account is active.</summary>
    ACTIVE,

    /// <summary>Account is a draft.</summary>
    DRAFT,

    /// <summary>Account was canceled.</summary>
    CANCELED,

    /// <summary>Account is pending deletion.</summary>
    PENDING_DELETION,

    /// <summary>Account was removed.</summary>
    REMOVED,
}
