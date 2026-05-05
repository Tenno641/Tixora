using Events.Domain.Common;
using Events.Domain.Tickets;

namespace Events.Domain.Categories;

public class Category: Entity
{
    public string Name { get; set; }
    public bool IsArchived { get; private set; }

    public static Category Create(
        string name,
        bool isArchived = false,
        Guid? id = null)
    {
        Category category = new Category(name, isArchived, id);

        return category;
    }

    public void Archive()
    {
        IsArchived = true;
        
        RaiseDomainEvent(new CategoryArchivedEvent(Id));
    }

    public void UpdateName(string newName)
    {
        if (Name == newName)
            return;
        
        Name = newName;
        
        RaiseDomainEvent(new CategoryNameUpdatedEvent(Id, newName));
    }

    private Category(string name, bool isArchived, Guid? id = null) : base(id)
    {
        Name = name;
    }
    
    private Category() { }
}