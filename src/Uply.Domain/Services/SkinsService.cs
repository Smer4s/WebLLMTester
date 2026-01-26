using MapsterMapper;
using Uply.Domain.Abstractions.Repositories;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Entities;
using Uply.Domain.Models._Skins_;
using Uply.Domain.Models.Dto.Skin;

namespace Uply.Domain.Services;

public class SkinsService(ISkinRepository skinRepository, IMapper mapper) : ISkinsService
{
    public async Task<IEnumerable<SkinDto>> GetSkins() =>
        (await skinRepository.GetAllAsync()).Select(mapper.Map<Skin, SkinDto>);

    public async Task<SkinDto> CreateSkin(CreateSkinDto skin)
    {
        if (await skinRepository.AnyAsync(s => s.Title == skin.Title))
        {
            throw new ArgumentException($"Название {skin.Title} уже использовано");
        }

        var skinDao = new Skin()
        {
            Description = skin.Description,
            Title = skin.Title,
            Price = skin.Price,
            SkinType = skin.SkinType,
            Url = skin.Url
        };

        await skinRepository.CreateAsync(skinDao);
        return mapper.Map<Skin, SkinDto>(skinDao);
    }

    public async Task<SkinDto> GetSkin(Guid id)
    {
        var model = await skinRepository.GetByIdAsync(id);
        
        return model != null 
            ? mapper.Map<Skin, SkinDto>(model) 
            : throw new Exception($"Скин {id} не был найден");
    }

    public async Task<SkinDto> UpdateSkin(UpdateSkinDto skin)
    {
        var skinDao = await skinRepository.GetByIdAsync(skin.Id);
        if (skinDao is null)
        {
            throw new Exception($"Скин {skin.Id} не был найден");
        }

        skinDao.Title = skin.Title;
        skinDao.Description = skin.Description;
        skinDao.SkinType = skin.SkinType;
        skinDao.Url = skin.Url;
        skinDao.Price = skin.Price;
        
        await skinRepository.UpdateAsync(skinDao);
        return mapper.Map<Skin, SkinDto>(skinDao);
    }
}