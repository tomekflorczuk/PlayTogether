using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using PlayTogether.Models;
using MySql.Data.MySqlClient;

namespace PlayTogether.Data
{
    public partial class PtContext : DbContext
    {
        public PtContext()
        {
        }

        public PtContext(DbContextOptions<PtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Participants> Participants { get; set; }
        public virtual DbSet<Places> Places { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SportTypes> SportTypes { get; set; }
        public virtual DbSet<Surfaces> Surfaces { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UpcomingGame> UpcomingGames { get; set; }
        public virtual DbSet<Cities> ListCities { get; set; }
        public virtual DbSet<Surfaces> ListSurfaces { get; set; }
        public virtual DbSet<GameParticipants> GameParticipants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                //   x => x.ServerVersion("5.7.14-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.CityId)
                    .HasName("PRIMARY");

                entity.Property(e => e.CityId)
                    .HasColumnName("city_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.CityName)
                    .HasColumnName("city_name")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });
            
            modelBuilder.Entity<Games>(entity =>
            {
                entity.HasKey(e => e.GameId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.GameType)
                    .HasName("fk_games_types");

                entity.HasIndex(e => e.HostUser)
                    .HasName("fk_games_players");

                entity.HasIndex(e => e.PlaceId)
                    .HasName("fk_games_places");

                entity.Property(e => e.GameId)
                    .HasColumnName("game_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Czas utworzenia wydarzenia");

                entity.Property(e => e.GameDate)
                    .HasColumnName("game_date")
                    .HasColumnType("datetime")
                    .HasComment("Data wydarzenia");

                entity.Property(e => e.GameLength)
                    .HasColumnName("game_length")
                    .HasColumnType("time")
                    .HasComment("Długość trwania wydarzenia");

                entity.Property(e => e.GameStatus)
                    .IsRequired()
                    .HasColumnName("game_status")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'A'")
                    .HasComment("A - aktywne, B - odwołane, D - zakończone")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.GameType)
                    .HasColumnName("game_type")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - typ wydarzenia, 1 - piłka nożna, 2 - koszykówka, 3 - siatkówka");

                entity.Property(e => e.HostUser)
                    .HasColumnName("host_user")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - Założyciel wydarzenia");

                entity.Property(e => e.MaxPlayers)
                    .HasColumnName("max_players")
                    .HasColumnType("int(11)")
                    .HasComment("Maksmalna liczba uczestników");

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Czas modyfikacji wydarzenia")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("varchar(256)")
                    .HasComment("Opis wydarzenia")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PlaceId)
                    .HasColumnName("place_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - miejsce wydarzenia");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("int(11)")
                    .HasComment("Cena od jednego uczestnika");

                entity.HasOne(d => d.GameTypeNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GameType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_games_types");

                entity.HasOne(d => d.HostUserNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.HostUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_games_players");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_games_places");
            });

            modelBuilder.Entity<Participants>(entity =>
            {
                entity.HasKey(e => e.ParticipantId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.GameId)
                    .HasName("fk_participant_game");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("fk_partcipant_player");

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("participant_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.Added)
                    .HasColumnName("added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Czas zapisania się do wydarzenia");

                entity.Property(e => e.GameId)
                    .HasColumnName("game_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - Wydarzenie");

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Czas modyfikacji wydarzenia")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ParticipantStatus)
                    .IsRequired()
                    .HasColumnName("participant_status")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'S'")
                    .HasComment("S - zapisany, U - wypisany")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - Zawodnik");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_participant_game");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_partcipant_user");
            });

            modelBuilder.Entity<Places>(entity =>
            {
                entity.HasKey(e => e.PlaceId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.PlaceName)
                    .HasName("place_name")
                    .IsUnique();

                entity.HasIndex(e => e.SurfaceId)
                    .HasName("fk_places_surfaces");

                entity.Property(e => e.PlaceId)
                    .HasColumnName("place_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasColumnName("place_name")
                    .HasColumnType("varchar(50)")
                    .HasComment("Nazwa/adres boiska")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CityId)
                    .HasColumnName("city_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - miasto wydarzenia");

                entity.Property(e => e.SurfaceId)
                    .HasColumnName("surface_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - typ nawierchni");

                entity.Property(e => e.X)
                    .HasColumnName("x")
                    .HasColumnType("int(11)")
                    .HasComment("Współrzędne położenia(x)");

                entity.Property(e => e.Y)
                    .HasColumnName("y")
                    .HasColumnType("int(11)")
                    .HasComment("Współrzędne położenia(y)");

                entity.HasOne(d => d.Surface)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.SurfaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_places_surfaces");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("PRIMARY");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.Bio)
                    .HasColumnName("bio")
                    .HasColumnType("varchar(256)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.GamesAttended)
                    .HasColumnName("games_attended")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PointsOfTrust)
                    .HasColumnName("points_of_trust")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProfilePicture)
                    .HasColumnName("profile_picture")
                    .HasColumnType("varchar(256)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.RoleName)
                    .HasName("role_name")
                    .IsUnique();

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<SportTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PRIMARY");

                entity.ToTable("Sport_types");

                entity.HasIndex(e => e.SportType)
                    .HasName("sport_type")
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.SportType)
                    .IsRequired()
                    .HasColumnName("sport_type")
                    .HasColumnType("varchar(12)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Surfaces>(entity =>
            {
                entity.HasKey(e => e.SurfaceId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SurfaceName)
                    .HasName("surface_name")
                    .IsUnique();

                entity.Property(e => e.SurfaceId)
                    .HasColumnName("surface_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.SurfaceName)
                    .IsRequired()
                    .HasColumnName("surface_name")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Email)
                    .HasName("email")
                    .IsUnique();

                entity.HasIndex(e => e.Login)
                    .HasName("login")
                    .IsUnique();

                entity.HasIndex(e => e.PlayerId)
                    .HasName("fk_user_player");

                entity.HasIndex(e => e.RoleId)
                    .HasName("fk_user_role");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Czas utworzenia konta");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Lastlogin)
                    .HasColumnName("lastlogin")
                    .HasColumnType("datetime")
                    .HasComment("Czas ostatniego zalogowania");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy- powiązany z uzytkownikiem zawodnik");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'3'")
                    .HasComment("Klucz obcy, 1-admin, 2-moderator, 3-user");

                entity.Property(e => e.UserStatus)
                    .IsRequired()
                    .HasColumnName("user_status")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'A'")
                    .HasComment("A - aktywny, B - zablokowany, D-usunięty")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_player");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_role");
            });

            modelBuilder.Entity<UpcomingGame>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.GameId).HasColumnName("game_id");
                entity.Property(e => e.PlaceId).HasColumnName("place_id");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.GameDate).HasColumnName("game_date");
                entity.Property(e => e.GameLength).HasColumnName("game_length");
                entity.Property(e => e.GameType).HasColumnName("game_type");
                entity.Property(e => e.MaxPlayers).HasColumnName("max_players");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Created).HasColumnName("created");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Modified).HasColumnName("modified");
                entity.Property(e => e.PlaceName).HasColumnName("place_name");
                entity.Property(e => e.CityName).HasColumnName("city_name");
                entity.Property(e => e.SurfaceName).HasColumnName("surface_name");
            });

            modelBuilder.Entity<Cities>(entity => { entity.ToView("ListCities"); });

            modelBuilder.Entity<Surfaces>(entity => { entity.ToView("ListSurfaces"); });

            modelBuilder.Entity<GameParticipants>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Amount).HasColumnName("amount");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public async Task<List<Users>> LoggingMethodAsync(string plogin, string ppassword, string pmail)
        {
            var list = new List<Users>();
            try
            {
                list = await Users.Where(u => u.Login == plogin && u.Email == pmail).ToListAsync();
                list = list.Where(u => u.Password == ppassword).ToList();
                //list = await Set<Users>().FromSqlRaw("CALL Logging (@p0, @p1, @p2)", new[] {plogin, ppassword, pmail}).ToListAsync();
            }
            catch (Exception ex)
            {
                
            }
            return list;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}