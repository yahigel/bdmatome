using bdaAPI.Repository;
using Microsoft.EntityFrameworkCore;

/*
 * データベースコンテキストは、Entity Framework (EF) Coreにおける中心的なクラスで、
 * データベースとのやり取りを管理します。EF Coreは、.NETで使われるオブジェクトリレーショナルマッピング (ORM) ツールの一つで、
 * オブジェクト指向のコードを使ってリレーショナルデータベースを扱うことを可能にします。
 * データベースコンテキストを使用することで、データベースのクエリ、挿入、更新、削除などの操作を行うことができます。
 * データベースコンテキストクラスはDbContextクラスを継承して定義されます。このクラス内では、
 * データベースの各テーブルに対応するDbSetプロパティを定義します。DbSetプロパティは、データベースのテーブルに格納されているデータを表すエンティティのコレクションを管理します。
 * エンティティは、データベースのテーブルの行に相当するクラスです。
 */
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnalyticalPerson> AnalyticalPerson { get; set; }
    public DbSet<Account> Account { get; set; }

}