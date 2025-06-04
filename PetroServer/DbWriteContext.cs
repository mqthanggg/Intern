using Microsoft.EntityFrameworkCore;

public class DbWrite(DbContextOptions<DbWrite> options): DbContext(options) {}