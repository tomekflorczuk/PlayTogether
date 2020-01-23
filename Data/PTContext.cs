using System;
using System.Collections.Generic;
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
        public virtual DbSet<GamesLog> GamesLog { get; set; }
        public virtual DbSet<Participants> Participants { get; set; }
        public virtual DbSet<ParticipantsLog> ParticipantsLog { get; set; }
        public virtual DbSet<Places> Places { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SportTypes> SportTypes { get; set; }
        public virtual DbSet<Surfaces> Surfaces { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersLog> UsersLog { get; set; }
        public virtual DbSet<UpcomingGame> UpcomingGames { get; set; }
        public virtual DbSet<Cities> ListCities { get; set; }
        public virtual DbSet<Surfaces> ListSurfaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=35.228.101.173;database=PlayTogether;uid=root;pwd=Hanusia22",
                    x => x.ServerVersion("5.7.14-mysql"));
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
                    .HasName("fk_games_users");

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
                    .HasColumnType("tinyint(4)")
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
                    .HasColumnType("tinyint(4)")
                    .HasComment("Klucz obcy - miejsce wydarzenia");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("tinyint(4)")
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
                    .HasConstraintName("fk_games_users");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_games_places");
            });

            modelBuilder.Entity<GamesLog>(entity =>
            {
                entity.HasKey(e => e.ModificationId)
                    .HasName("PRIMARY");

                entity.Property(e => e.ModificationId)
                    .HasColumnName("modification_id")
                    .HasColumnType("int(11)")
                    .HasComment("Primary key");

                entity.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModificationType)
                    .IsRequired()
                    .HasColumnName("modification_type")
                    .HasColumnType("char(1)")
                    .HasComment("U-update, I-insert, D-delete")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modifier)
                    .IsRequired()
                    .HasColumnName("modifier")
                    .HasColumnType("varchar(25)")
                    .HasComment("Użytkownik wprowadzający zmianę")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewGameDate)
                    .HasColumnName("new_game_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewGameId)
                    .HasColumnName("new_game_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewGameLength)
                    .HasColumnName("new_game_length")
                    .HasColumnType("time");

                entity.Property(e => e.NewGameStatus)
                    .HasColumnName("new_game_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewGameType)
                    .HasColumnName("new_game_type")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.NewHostUser)
                    .HasColumnName("new_host_user")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewMaxPlayers)
                    .HasColumnName("new_max_players")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewNotes)
                    .HasColumnName("new_notes")
                    .HasColumnType("varchar(256)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewPlaceId)
                    .HasColumnName("new_place_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewPrice)
                    .HasColumnName("new_price")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.OldGameDate)
                    .HasColumnName("old_game_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OldGameId)
                    .HasColumnName("old_game_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldGameLength)
                    .HasColumnName("old_game_length")
                    .HasColumnType("time");

                entity.Property(e => e.OldGameStatus)
                    .HasColumnName("old_game_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldGameType)
                    .HasColumnName("old_game_type")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.OldHostUser)
                    .HasColumnName("old_host_user")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldMaxPlayers)
                    .HasColumnName("old_max_players")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldNotes)
                    .HasColumnName("old_notes")
                    .HasColumnType("varchar(256)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldPlaceId)
                    .HasColumnName("old_place_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldPrice)
                    .HasColumnName("old_price")
                    .HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<Participants>(entity =>
            {
                entity.HasKey(e => e.ParticipantId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.GameId)
                    .HasName("fk_participant_game");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_partcipant_user");

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

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasComment("Klucz obcy - Zawodnik");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_participant_game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_partcipant_user");
            });

            modelBuilder.Entity<ParticipantsLog>(entity =>
            {
                entity.HasKey(e => e.ModificationId)
                    .HasName("PRIMARY");

                entity.Property(e => e.ModificationId)
                    .HasColumnName("modification_id")
                    .HasColumnType("int(11)")
                    .HasComment("Primary key");

                entity.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModificationType)
                    .IsRequired()
                    .HasColumnName("modification_type")
                    .HasColumnType("char(1)")
                    .HasComment("U-update, I-insert, D-delete")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modifier)
                    .IsRequired()
                    .HasColumnName("modifier")
                    .HasColumnType("varchar(25)")
                    .HasComment("Użytkownik wprowadzający zmianę")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewGameId)
                    .HasColumnName("new_game_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewParticiapntId)
                    .HasColumnName("new_particiapnt_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewParticipantStatus)
                    .HasColumnName("new_participant_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewUserId)
                    .HasColumnName("new_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldGameId)
                    .HasColumnName("old_game_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldParticipantId)
                    .HasColumnName("old_participant_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldParticipantStatus)
                    .HasColumnName("old_participant_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldUserId)
                    .HasColumnName("old_user_id")
                    .HasColumnType("int(11)");
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
                    .HasColumnType("tinyint(4)")
                    .HasComment("Klucz główny");

                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasColumnName("place_name")
                    .HasColumnType("varchar(25)")
                    .HasComment("Nazwa/adres boiska")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SurfaceId)
                    .HasColumnName("surface_id")
                    .HasColumnType("tinyint(4)")
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
                    .HasColumnType("tinyint(4)")
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
                    .HasColumnType("tinyint(4)")
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
                    .HasColumnType("tinyint(4)")
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
                    .HasColumnType("tinyint(4)")
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

            modelBuilder.Entity<UsersLog>(entity =>
            {
                entity.HasKey(e => e.ModificationId)
                    .HasName("PRIMARY");

                entity.Property(e => e.ModificationId)
                    .HasColumnName("modification_id")
                    .HasColumnType("int(11)")
                    .HasComment("Primary key");

                entity.Property(e => e.ModificationTime)
                    .HasColumnName("modification_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModificationType)
                    .IsRequired()
                    .HasColumnName("modification_type")
                    .HasColumnType("char(1)")
                    .HasComment("U-update, I-insert, D-delete")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Modifier)
                    .IsRequired()
                    .HasColumnName("modifier")
                    .HasColumnType("varchar(25)")
                    .HasComment("Użytkownik wprowadzający zmianę")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewEmail)
                    .HasColumnName("new_email")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewLogin)
                    .HasColumnName("new_login")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewPassword)
                    .HasColumnName("new_password")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NewPlayerId)
                    .HasColumnName("new_player_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewRoleId)
                    .HasColumnName("new_role_id")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.NewUserId)
                    .HasColumnName("new_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewUserStatus)
                    .HasColumnName("new_user_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldEmail)
                    .HasColumnName("old_email")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldLogin)
                    .HasColumnName("old_login")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldPassword)
                    .HasColumnName("old_password")
                    .HasColumnType("varchar(25)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OldPlayerId)
                    .HasColumnName("old_player_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldRoleId)
                    .HasColumnName("old_role_id")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.OldUserId)
                    .HasColumnName("old_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OldUserStatus)
                    .HasColumnName("old_user_status")
                    .HasColumnType("char(1)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Logging>(entity => { entity.HasNoKey(); });

            modelBuilder.Entity<UpcomingGame>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("UpcomingActiveGames");
                entity.Property(v => v.HostUser).HasColumnName("host_user");
                entity.Property(v => v.GameDate).HasColumnName("game_date");
                entity.Property(v => v.GameLength).HasColumnName("game_length");
                entity.Property(v => v.GameType).HasColumnName("game_type");
                entity.Property(v => v.MaxPlayers).HasColumnName("max_players");
                entity.Property(v => v.Price).HasColumnName("price");
                entity.Property(v => v.PlaceId).HasColumnName("place_id");
                entity.Property(v => v.Created).HasColumnName("created");
                entity.Property(v => v.Notes).HasColumnName("notes");
            });

            modelBuilder.Entity<Cities>(entity => { entity.ToView("ListCities"); });

            modelBuilder.Entity<Surfaces>(entity => { entity.ToView("ListSurfaces"); });

            modelBuilder.Entity<Games>(entity =>
            {
                entity.ToView("UpcomingFootballActiveGames");
                entity.Property(e => e.GameId).HasColumnName("game_id");
                entity.Property(v => v.HostUser).HasColumnName("host_user");
                entity.Property(v => v.GameDate).HasColumnName("game_date");
                entity.Property(v => v.GameLength).HasColumnName("game_length");
                entity.Property(v => v.GameType).HasColumnName("game_type");
                entity.Property(v => v.MaxPlayers).HasColumnName("max_players");
                entity.Property(v => v.Price).HasColumnName("price");
                entity.Property(v => v.PlaceId).HasColumnName("place_id");
                entity.Property(v => v.Created).HasColumnName("created");
                entity.Property(v => v.Notes).HasColumnName("notes");
                entity.Property(v => v.Modified).HasColumnName("modified");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public async Task<List<Logging>> LoggingMethodAsync(string plogin, string ppassword, string pmail)
        {
            var list = new List<Logging>();

            try
            {
                list = await Set<Logging>().FromSqlRaw("CALL Logging (@p0, @p1, @p2)", new[] {plogin, ppassword, pmail})
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}