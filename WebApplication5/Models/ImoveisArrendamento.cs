using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class ImoveisArrendamento
{
    public Guid Id { get; set; }

    public Guid? AtivoId { get; set; }

    public string Designacao { get; set; } = null!;

    public Guid? LocalizacaoId { get; set; }

    public double ValorPropriedade { get; set; }

    public double ValorRenda { get; set; }

    public double TaxaCondominio { get; set; }

    public double DespesasAnuais { get; set; }

    public double PercentagemPropriedade { get; set; }

    public virtual AtivosFinanceiro? Ativo { get; set; }

    public virtual Localizaco? Localizacao { get; set; }
}
