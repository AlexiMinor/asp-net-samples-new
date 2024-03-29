﻿using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class Role : IBaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public List<User> Users { get; set; }
}