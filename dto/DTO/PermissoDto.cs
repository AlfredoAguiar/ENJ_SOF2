namespace WebApplication5.DTO;

public class PermissoDto
{
    public Guid Id { get; set; }
    public bool? PodeCriarUtilizadores { get; set; }
    public bool? PodeEditarUtilizadores { get; set; }
    public bool? PodeVerRegistos { get; set; }
    public bool? PodeAlterarPermissoes { get; set; }
}