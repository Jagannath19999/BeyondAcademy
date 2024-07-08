﻿namespace BeyondAcademy.Interface
{
    public interface IRoleService
    {
        string GetRoleNameForAcId(string acId);
        string HashPassword(string password);
    }
}
