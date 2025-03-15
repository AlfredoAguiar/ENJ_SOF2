using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Permisso
{
    public Guid Id { get; set; }

    public bool? PodeCriarUtilizadores { get; set; }

    public bool? PodeEditarUtilizadores { get; set; }

    public bool? PodeVerRegistos { get; set; }

    public bool? PodeAlterarPermissoes { get; set; }

    public virtual ICollection<Utilizadore> Utilizadores { get; set; } = new List<Utilizadore>();
}
