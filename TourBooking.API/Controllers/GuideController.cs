using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Domain.Entities;
using TourBooking.Infrastructure.Context;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/guides/tour-price")]
    public class GuideController : BaseController
    {
        private readonly AppDbContext _db;

        public GuideController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuideTourPriceDto>>> List(
            [FromQuery] Guid? cityId,
            [FromQuery] Guid? districtId,
            [FromQuery] Guid? tourPointId
        )
        {
            var guideId = GetUserIdFromToken();

            var q = _db.GuideTourPrices.AsQueryable().Where(x => x.GuideId == guideId);
            if (cityId is not null)
                q = q.Where(x => x.CityId == cityId);
            if (districtId is not null)
                q = q.Where(x => x.DistrictId == districtId);
            if (tourPointId is not null)
                q = q.Where(x => x.TourPointId == tourPointId);

            var list = await q.OrderByDescending(x => x.CreatedAt)
                .Select(x => new GuideTourPriceDto(
                    x.Id,
                    x.GuideId,
                    x.CityId,
                    x.DistrictId,
                    x.TourPointId,
                    x.Price,
                    x.Currency,
                    x.Commission
                ))
                .ToListAsync();

            return Ok(new { items = list });
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<GuideTourPriceDto>> Upsert(
            [FromBody] UpsertGuideTourPriceRequest req
        )
        {
            var guideId = GetUserIdFromToken();
            if (req.DistrictId is not null && req.CityId is null)
                return BadRequest("DistrictId verildiyse CityId zorunlu.");

            var existing = await _db.GuideTourPrices.FirstOrDefaultAsync(x =>
                x.GuideId == guideId
                && x.CityId == req.CityId
                && x.DistrictId == req.DistrictId
                && x.TourPointId == req.TourPointId
            );

            if (existing is null)
            {
                var entity = new GuideTourPriceEntity
                {
                    Id = Guid.NewGuid(),
                    GuideId = guideId,
                    CityId = req.CityId,
                    DistrictId = req.DistrictId,
                    TourPointId = req.TourPointId,
                    Price = req.Price,
                    Currency = req.Currency ?? "TRY",
                    Commission=req.Commission
                };
                _db.GuideTourPrices.Add(entity);
                await _db.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { guideId, id = entity.Id },
                    new GuideTourPriceDto(
                        entity.Id,
                        guideId,
                        entity.CityId,
                        entity.DistrictId,
                        entity.TourPointId,
                        entity.Price,
                        entity.Currency,
                        entity.Commission
                    )
                );
            }
            else
            {
                existing.Price = req.Price;
                if (!string.IsNullOrWhiteSpace(req.Currency))
                    existing.Currency = req.Currency!;
                await _db.SaveChangesAsync();

                return Ok(
                    new GuideTourPriceDto(
                        existing.Id,
                        guideId,
                        existing.CityId,
                        existing.DistrictId,
                        existing.TourPointId,
                        existing.Price,
                        existing.Currency,
                        existing.Commission
                    )
                );
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GuideTourPriceDto>> GetById(Guid id)
        {
            var guideId = GetUserIdFromToken();
            var x = await _db.GuideTourPrices.FirstOrDefaultAsync(p =>
                p.Id == id && p.GuideId == guideId
            );
            if (x is null)
                return NotFound();
            return new GuideTourPriceDto(
                x.Id,
                guideId,
                x.CityId,
                x.DistrictId,
                x.TourPointId,
                x.Price,
                x.Currency,
                x.Commission
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpsertGuideTourPriceRequest req)
        {
            var guideId = GetUserIdFromToken();
            var x = await _db.GuideTourPrices.FirstOrDefaultAsync(p =>
                p.Id == id && p.GuideId == guideId
            );
            if (x is null)
                return NotFound();

            if (req.DistrictId is not null && req.CityId is null)
                return BadRequest("DistrictId verildiyse CityId zorunlu.");

            x.CityId = req.CityId;
            x.DistrictId = req.DistrictId;
            x.TourPointId = req.TourPointId;
            x.Price = req.Price;
            x.Commission = req.Commission;
            if (!string.IsNullOrWhiteSpace(req.Currency))
                x.Currency = req.Currency!;
            try
            {
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict("Aynı kombinasyon zaten mevcut (unique).");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var guideId = GetUserIdFromToken();
            var x = await _db.GuideTourPrices.FirstOrDefaultAsync(p =>
                p.Id == id && p.GuideId == guideId
            );
            if (x is null)
                return NotFound();
            _db.Remove(x);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("getMyLanguages")]
        public async Task<ActionResult<object>> GetMyLanguages()
        {
            var guideId = GetUserIdFromToken();
            var ids = await _db
                .GuideLanguages.Where(gl => gl.GuideId == guideId)
                .Select(gl => gl.LanguageId)
                .ToListAsync();

            // var languages = await _db.Guides
            //     .Where(g => g.Id == guideId) 
            //     .SelectMany(g => g.GuideLanguages)
            //     .Select(gl => gl.Language.Id) 
            //     .ToListAsync();
            /// 11111111111!!!!!!!!!

            return Ok(new { languageIds = ids });
        }

        [HttpPut("saveMyLanguages")]
        public async Task<IActionResult> ReplaceMine([FromBody] GuideLanguageUpdateRequest req)
        {
            var guideId = GetUserIdFromToken();

            // 0) Guide var mı? (FK patlamasın)
            var guideExists = await _db.Guides
                .AsNoTracking()
                .AnyAsync(g => g.Id == guideId);
            if (!guideExists)
                return NotFound(new { message = "Guide profili bulunamadı. Önce guide kaydı oluşturun." });

            // 1) normalize
            var newIds = (req.LanguageIds ?? new List<Guid>())
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            // 2) geçerli dil ID'leri (yoksa 400)
            var validNewIds = await _db.Languages
                .Where(l => newIds.Contains(l.Id))
                .Select(l => l.Id)
                .ToListAsync();

            var invalidIds = newIds.Except(validNewIds).ToList();
            if (invalidIds.Count > 0)
                return BadRequest(new { message = "Geçersiz dil ID'leri", invalidIds });

            // 3) mevcut seçim (sadece ID’ler)
            var currentIds = await _db.GuideLanguages
                .Where(gl => gl.GuideId == guideId)
                .Select(gl => gl.LanguageId)
                .ToListAsync();

            // 4) delta
            var toAddIds = validNewIds.Except(currentIds).ToList();
            var toRemoveIds = currentIds.Except(validNewIds).ToList();

            // 5) uygula — composite key ise entity’i stub’layıp attach/remove yap
            if (toRemoveIds.Count > 0)
            {
                var removeStubs = toRemoveIds
                    .Select(id => new GuideLanguageEntity { GuideId = guideId, LanguageId = id })
                    .ToList();
                _db.AttachRange(removeStubs);
                _db.RemoveRange(removeStubs);
            }

            if (toAddIds.Count > 0)
            {
                var addRows = toAddIds
                    .Select(id => new GuideLanguageEntity
                    {
                        GuideId = guideId,
                        LanguageId = id
                        // Eğer composite PK DEĞİL ve PK Id ise:
                        // Id = Guid.NewGuid()
                    })
                    .ToList();

                await _db.GuideLanguages.AddRangeAsync(addRows);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
