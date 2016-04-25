namespace Tar
{
    public enum TarEntryType : byte
    {
        File = (byte)'0',
        HardLink,
        SymbolicLink,
        CharacterDevice,
        BlockDevice,
        Directory,
        FIFO,
    }
}
