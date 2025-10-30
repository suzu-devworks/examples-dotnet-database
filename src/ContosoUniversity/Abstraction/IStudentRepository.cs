using ContosoUniversity.Models;

namespace ContosoUniversity.Abstraction;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> FindAllAsync(CancellationToken cancelToken = default);

    Task<Student?> FindAsync(int id, CancellationToken cancelToken = default);

    Task AddAsync(Student student, CancellationToken cancelToken = default);

    Task UpdateAsync(int id, Student modifier, CancellationToken cancelToken = default);

    Task RemoveAsync(int id, CancellationToken cancelToken = default);

}
