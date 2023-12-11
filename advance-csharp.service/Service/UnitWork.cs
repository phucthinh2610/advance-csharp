using advance_csharp.service.Interface;
using advance_csharp.database;


namespace advance_csharp.service.Service
{
    public class UnitWork : IUnitWork
    {
        private readonly AdvanceCsharpContext _context;
        public UnitWork(AdvanceCsharpContext context)
        {

            _context = context;
        }

        /// <summary>
        /// CompleteAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CompleteAsync(string email)
        {
            return await _context.SaveChangesAsync(email) > 0;
        }
    }

}