using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class Dbes2 : DbContext
{
    public Dbes2()
    {
    }

    public Dbes2(DbContextOptions<Dbes2> options)
        : base(options)
    {
    }

    public virtual DbSet<AtivosFinanceiro> AtivosFinanceiros { get; set; }

    public virtual DbSet<Carteira> Carteiras { get; set; }

    public virtual DbSet<Deposito> Depositos { get; set; }

    public virtual DbSet<FundosInvestimento> FundosInvestimentos { get; set; }

    public virtual DbSet<ImoveisArrendamento> ImoveisArrendamentos { get; set; }

    public virtual DbSet<Localizaco> Localizacoes { get; set; }

    public virtual DbSet<Permisso> Permissoes { get; set; }

    public virtual DbSet<Utilizadore> Utilizadores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=es2;Username=postgres;Password=es2");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<AtivosFinanceiro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ativos_financeiros_pkey");

            entity.ToTable("ativos_financeiros");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.TaxaPercentagem).HasColumnName("taxa_percentagem");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Carteira>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("carteira_pkey");

            entity.ToTable("carteira");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.Montante).HasColumnName("montante");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");

            entity.HasOne(d => d.Ativo).WithMany(p => p.Carteiras)
                .HasForeignKey(d => d.AtivoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("carteira_ativo_id_fkey");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Carteiras)
                .HasForeignKey(d => d.UtilizadorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("carteira_utilizador_id_fkey");
        });

        modelBuilder.Entity<Deposito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("depositos_pkey");

            entity.ToTable("depositos");

            entity.HasIndex(e => e.NumeroConta, "depositos_numero_conta_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.Banco)
                .HasMaxLength(100)
                .HasColumnName("banco");
            entity.Property(e => e.Montante).HasColumnName("montante");
            entity.Property(e => e.NumeroConta)
                .HasMaxLength(50)
                .HasColumnName("numero_conta");
            entity.Property(e => e.TaxaJuros).HasColumnName("taxa_juros");
            entity.Property(e => e.Titulares).HasColumnName("titulares");

            entity.HasOne(d => d.Ativo).WithMany(p => p.Depositos)
                .HasForeignKey(d => d.AtivoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("depositos_ativo_id_fkey");
        });

        modelBuilder.Entity<FundosInvestimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fundos_investimento_pkey");

            entity.ToTable("fundos_investimento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.DuracaoMeses).HasColumnName("duracao_meses");
            entity.Property(e => e.MontanteInvestido).HasColumnName("montante_investido");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.TaxaJurosPadrao).HasColumnName("taxa_juros_padrao");

            entity.HasOne(d => d.Ativo).WithMany(p => p.FundosInvestimentos)
                .HasForeignKey(d => d.AtivoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fundos_investimento_ativo_id_fkey");
        });

        modelBuilder.Entity<ImoveisArrendamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("imoveis_arrendamento_pkey");

            entity.ToTable("imoveis_arrendamento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
            entity.Property(e => e.DespesasAnuais).HasColumnName("despesas_anuais");
            entity.Property(e => e.LocalizacaoId).HasColumnName("localizacao_id");
            entity.Property(e => e.PercentagemPropriedade).HasColumnName("percentagem_propriedade");
            entity.Property(e => e.TaxaCondominio).HasColumnName("taxa_condominio");
            entity.Property(e => e.ValorPropriedade).HasColumnName("valor_propriedade");
            entity.Property(e => e.ValorRenda).HasColumnName("valor_renda");

            entity.HasOne(d => d.Ativo).WithMany(p => p.ImoveisArrendamentos)
                .HasForeignKey(d => d.AtivoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("imoveis_arrendamento_ativo_id_fkey");

            entity.HasOne(d => d.Localizacao).WithMany(p => p.ImoveisArrendamentos)
                .HasForeignKey(d => d.LocalizacaoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("imoveis_arrendamento_localizacao_id_fkey");
        });

        modelBuilder.Entity<Localizaco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("localizacoes_pkey");

            entity.ToTable("localizacoes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cidade)
                .HasMaxLength(100)
                .HasColumnName("cidade");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(20)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Permisso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissoes_pkey");

            entity.ToTable("permissoes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.PodeAlterarPermissoes)
                .HasDefaultValue(false)
                .HasColumnName("pode_alterar_permissoes");
            entity.Property(e => e.PodeCriarUtilizadores)
                .HasDefaultValue(false)
                .HasColumnName("pode_criar_utilizadores");
            entity.Property(e => e.PodeEditarUtilizadores)
                .HasDefaultValue(false)
                .HasColumnName("pode_editar_utilizadores");
            entity.Property(e => e.PodeVerRegistos)
                .HasDefaultValue(true)
                .HasColumnName("pode_ver_registos");
        });

        modelBuilder.Entity<Utilizadore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilizadores_pkey");

            entity.ToTable("utilizadores");

            entity.HasIndex(e => e.Email, "utilizadores_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .HasColumnName("cargo");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.PermissaoId).HasColumnName("permissao_id");
            entity.Property(e => e.Senha).HasColumnName("senha");

            entity.HasOne(d => d.Permissao).WithMany(p => p.Utilizadores)
                .HasForeignKey(d => d.PermissaoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("utilizadores_permissao_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
