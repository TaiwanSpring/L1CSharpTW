namespace LineageServer.Server.Templates
{
    class L1DoorGfx
    {
        public int GfxId { get; }

        public int Direction { get; }

        public int RightEdgeOffset { get; }

        public int LeftEdgeOffset { get; }

        public L1DoorGfx(int gfxId, int direction, int rightEdgeOffset, int leftEdgeOffset)
        {
            GfxId = gfxId;
            Direction = direction;
            RightEdgeOffset = rightEdgeOffset;
            LeftEdgeOffset = leftEdgeOffset;
        }
    }
}