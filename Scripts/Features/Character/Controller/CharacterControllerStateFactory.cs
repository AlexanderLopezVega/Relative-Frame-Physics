namespace Project
{
    public class CharacterControllerStateFactory
    {
        //  Fields
        private readonly CharacterController _context;

        //  Constructors
        public CharacterControllerStateFactory(CharacterController context)
        {
            _context = context;
        }

        //  Methods
        public CharacterControllerState CreateGroundState() => new GroundState(_context);
        public CharacterControllerState CreateAirState() => new AirState(_context);
    }
}
