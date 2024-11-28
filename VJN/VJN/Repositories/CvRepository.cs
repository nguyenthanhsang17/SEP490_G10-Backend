using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class CvRepository : ICvRepository
    {
        private readonly VJNDBContext _context;
        public CvRepository(VJNDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cv>> GetCvByUserID(int user)
        {
            var cvs = await _context.Cvs.Where(c => c.UserId == user).Include(c => c.ItemOfCvs).ToListAsync();
            return cvs;
        }

        public async Task<IEnumerable<Cv>> GetCvAllcv()
        {
            var cvs = await _context.Cvs.Include(c => c.ItemOfCvs).ToListAsync();
            return cvs;
        }

        public async Task<bool> UpdateCV(List<Cv> cvs, int userid)
        {
            // Xóa liên kết UserId với các Cv hiện tại
            var cvu = await _context.Cvs.Where(cv => cv.UserId == userid).Select(cv => cv.CvId).ToListAsync();
            Console.WriteLine("sang :::" + string.Join(",", cvu));

            if (cvu.Count > 0)
            {
                var cvsToUpdate = await _context.Cvs.Where(cv => cv.UserId == userid).ToListAsync();
                foreach (var cv in cvsToUpdate)
                {
                    cv.UserId = null;  // Đặt UserId là null cho các Cv hiện tại
                }
                await _context.SaveChangesAsync();  // Lưu lại sau khi xóa UserId
            }

            List<int> cvId = new List<int>();

            // Thêm các Cv mới
            foreach (var cv in cvs)
            {
                var newCv = new Cv
                {
                    UserId = userid,
                    NameCv = cv.NameCv,
                };
                _context.Cvs.Add(newCv);  // Thêm Cv vào context
                await _context.SaveChangesAsync();  // Lưu các Cv, giá trị CvId sẽ được sinh tự động
                cv.CvId = newCv.CvId;
            }



            // Thêm các bản ghi ItemOfCv với CvId mới
            foreach (var cv in cvs)
            {
                foreach (var item in cv.ItemOfCvs)
                {
                    item.CvId = cv.CvId;
                    var newitem = new ItemOfCv()
                    {
                        CvId = item.CvId,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                    };
                    _context.ItemOfCvs.Add(item);  // Thêm ItemOfCv vào context
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<bool> CreateCv(Cv cv)
        {
            var newCv = new Cv
            {
                UserId = cv.UserId,
                NameCv = cv.NameCv,
            };
            _context.Cvs.Add(newCv);
            await _context.SaveChangesAsync();
            cv.CvId = newCv.CvId;

            foreach (var item in cv.ItemOfCvs)
            {
                item.CvId = cv.CvId;
                var newitem = new ItemOfCv()
                {
                    CvId = item.CvId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                };
                _context.ItemOfCvs.Add(item);  // Thêm ItemOfCv vào context
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UpdateCv(Cv cv)
        {
            // Tìm CV hiện tại trong DB
            var existingCv = await _context.Cvs
                .Include(c => c.ItemOfCvs)
                .FirstOrDefaultAsync(c => c.CvId == cv.CvId);

            if (existingCv == null)
                return false;

            // Kiểm tra sự thay đổi
            bool hasChanges = false;

            // So sánh tên CV
            if (!string.Equals(existingCv.NameCv, cv.NameCv, StringComparison.OrdinalIgnoreCase))
            {
                hasChanges = true;
            }

            // So sánh ItemOfCvs
            if (existingCv.ItemOfCvs.Count != cv.ItemOfCvs.Count)
            {
                hasChanges = true;
            }
            else
            {
                // So sánh chi tiết từng ItemOfCv
                foreach (var newItem in cv.ItemOfCvs)
                {
                    var matchingExistingItem = existingCv.ItemOfCvs
                        .FirstOrDefault(ei =>
                                ei.ItemName.Equals(newItem.ItemName) &&
                                ei.ItemDescription.Equals(newItem.ItemDescription));

                    if (matchingExistingItem == null)
                    {
                        hasChanges = true;
                        break;
                    }
                }
            }
            if (!hasChanges)
            {
                return true;
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var sang = existingCv.UserId;
                    // Đặt UserId của CV cũ về null
                    existingCv.UserId = null;
                    await _context.SaveChangesAsync();

                    // Tạo CV mới
                    var newCv = new Cv
                    {
                        UserId = sang, // Lấy UserId của CV cũ
                        NameCv = cv.NameCv,
                        // Sao chép các thuộc tính khác nếu cần
                    };

                    // Thêm CV mới vào context
                    _context.Cvs.Add(newCv);
                    await _context.SaveChangesAsync();

                    // Tạo ItemOfCvs mới
                    var newItemOfCvs = cv.ItemOfCvs.Select(item => new ItemOfCv
                    {
                        CvId = newCv.CvId, // Sử dụng ID của CV mới
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription
                    }).ToList();

                    // Thêm ItemOfCvs mới
                    _context.ItemOfCvs.AddRange(newItemOfCvs);
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DeleteCv(int cvid)
        {
            var cvb = await _context.Cvs.Where(cv => cv.CvId == cvid).SingleOrDefaultAsync();
            if (cvb == null)
            {
                return true;
            }
            cvb.UserId = null;
            _context.Entry(cvb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
