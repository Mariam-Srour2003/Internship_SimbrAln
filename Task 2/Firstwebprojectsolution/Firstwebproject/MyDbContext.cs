﻿using Microsoft.EntityFrameworkCore;


public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }


}