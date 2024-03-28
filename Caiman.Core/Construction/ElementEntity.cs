namespace Caiman.Core.Construction;

public class ElementEntity(NodeEntity startNodeEntity, NodeEntity endNodeEntity, double elasticity, double area)
{
    public NodeEntity StartNodeEntity { get; set; } = startNodeEntity;

    public NodeEntity EndNodeEntity { get; set; } = endNodeEntity;

    /// <summary>
    ///     Модуль упругости, в кг/см^2
    /// </summary>
    public double Elasticity { get; set; } = elasticity;

    /// <summary>
    ///     Площадь поперечного сечения, в см^2
    /// </summary>
    public double Area { get; set; } = area;

    public double Stiffness => Elasticity * Area / Length;

    public double Dx => EndNodeEntity.X - StartNodeEntity.X;

    public double Dy => EndNodeEntity.Y - StartNodeEntity.Y;

    public double Length => Math.Sqrt(Dx * Dx + Dy * Dy);

    public double Sin => Dy / Length;

    public double Cos => Dx / Length;

    // public override string ToString() =>
    //     $"ElementEntity:\n\tId: {Id}, Start NodeEntity: {StartNodeEntity.Id}, End NodeEntity: {EndNodeEntity.Id}, Elasticity: {Elasticity}, Area: {Area}";
}
