namespace Team11.Interactions
{
    public interface IContainer
    {
        public bool PutIn(IPickup pickup);
        public IPickup TakeOut();
    }
}