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
            var cvs = await _context.Cvs.Where(c => c.UserId == user).Include(c=>c.ItemOfCvs).ToListAsync();
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

            // Thêm các Cv mới
            foreach (var cv in cvs)
            {
                cv.UserId = userid;  // Gán UserId cho Cv mới
                _context.Cvs.Add(cv);  // Thêm Cv vào context
                await _context.SaveChangesAsync();  // Lưu các Cv, giá trị CvId sẽ được sinh tự động
            }

           

            // Thêm các bản ghi ItemOfCv với CvId mới
            foreach (var cv in cvs)
            {
                foreach (var item in cv.ItemOfCvs)
                {
                    item.CvId = cv.CvId;  // Gán CvId cho ItemOfCv
                    _context.ItemOfCvs.Add(item);  // Thêm ItemOfCv vào context
                    await _context.SaveChangesAsync();
                }
            }
            return true;
        }
    }
}
