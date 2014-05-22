using SharpSvn;

namespace SvnClientLib
{
    public interface IItemValidator
    {
        bool Validate(SvnChangeItem item);
    }
}