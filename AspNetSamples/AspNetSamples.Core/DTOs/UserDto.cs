﻿namespace AspNetSamples.Core.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}