﻿using System.Collections.Generic;

namespace BlogSimple.BlazorApp.Models;

public class AuthenResponseModel
{
    public string Id { get; set; }
    public string AuthenToken { get; set; }
    public int ExpiresIn { get; set; }
    public List<string> Roles { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string RefreshTokenHash { get; set; }
    public bool Successful { get; set; }
}