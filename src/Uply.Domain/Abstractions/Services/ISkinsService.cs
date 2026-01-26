using Uply.Domain.Models._Skins_;
using Uply.Domain.Models.Dto.Skin;

namespace Uply.Domain.Abstractions.Services;

public interface ISkinsService
{
    Task<IEnumerable<SkinDto>> GetSkins();

    Task<SkinDto> CreateSkin(CreateSkinDto skin);
    
    Task<SkinDto> GetSkin(Guid id);

    Task<SkinDto> UpdateSkin(UpdateSkinDto skin);
}