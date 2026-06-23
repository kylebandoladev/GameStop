using GameStop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStop.Api.Data;

public class GameStopContext(DbContextOptions<GameStopContext> options) 
    : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
}