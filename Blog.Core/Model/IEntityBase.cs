using System;

namespace Blog.Core.Model;

public class IEntityBase
{
    public Guid Id { get; set; }
    public DateTime CreateOn { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime? DeleteOn { get; set; }
    public Guid? DeleteBy { get; set; }
    public DateTime? ChangeOn { get; set; }
    public Guid? ChangeBy { get; set; }
}