namespace LineageServer.Server.Server.Utils
{
	public class Dice
	{
		private readonly int _faces;

		public Dice(int faces)
		{
			_faces = faces;
		}

		public virtual int Faces
		{
			get
			{
				return _faces;
			}
		}

		public virtual int roll()
		{
			return RandomHelper.Next(_faces) + 1;
		}

		public virtual int roll(int count)
		{
			int n = 0;
			for (int i = 0; i < count; i++)
			{
				n += roll();
			}
			return n;
		}
	}

}