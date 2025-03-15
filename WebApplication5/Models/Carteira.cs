using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Carteira
{
    public Guid Id { get; set; }

    public Guid? UtilizadorId { get; set; }

    public Guid? AtivoId { get; set; }

    public DateOnly DataInicio { get; set; }

    public double Montante { get; set; }

    public virtual AtivosFinanceiro? Ativo { get; set; }

    public virtual Utilizadore? Utilizador { get; set; }
}
