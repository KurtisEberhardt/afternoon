
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using afternoon.Models;

namespace afternoon.Repos
{
    public class AdmissionsRepo
    {
        private readonly IDbConnection _db;

        public AdmissionsRepo(IDbConnection db)
        {
            _db = db;
        }

        internal IEnumerable<Admission> GetAll()
        {

            // string sql = @"SELECT * from admissions";
            // return _db.Query<Admission>(sql);

            string sql = @"
            SELECT 
            a.*,
            p.*
            FROM admissions a
            JOIN profiles p ON a.buyerId = p.id;";
            return _db.Query<Admission, Profile, Admission>(sql, (admission, profile) =>
                {
                    admission.Buyer = profile;
                    return admission;
                }, splitOn: "id");
        }

        internal Admission GetById(int Id)
        {
            // string sql = @"SELECT * from admissions WHERE id = @Id;";
            // return _db.QueryFirstOrDefault<Admission>(sql, new { Id });

            string sql = @"
  SELECT 
  a.*,
  p.*
  FROM admissions a
  JOIN profiles p ON a.buyerId = p.id
  WHERE a.id = @Id;";
            return _db.Query<Admission, Profile, Admission>(sql, (admission, profile) =>
                {
                    admission.Buyer = profile;
                    return admission;
                }, new { Id }, splitOn: "id").FirstOrDefault();


        }

        internal Admission Create(Admission newAdmission)
        {
            string sql = @"
      INSERT INTO admissions
      (date, buyerId)
      VALUES
      (@Date, @BuyerId);
      SELECT LAST_INSERT_ID();";
            int id = _db.ExecuteScalar<int>(sql, newAdmission);
            newAdmission.Id = id;
            return newAdmission;
        }

        internal Admission Edit(Admission updatedAdmission)
        {
            string sql = @"
      UPDATE admissions
      SET
        date = @Date
      WHERE id = @Id;
      SELECT * FROM admissions WHERE id = @Id;";
            Admission returnAdmission = _db.QueryFirstOrDefault<Admission>(sql, updatedAdmission);
            return returnAdmission;
        }


        internal void Delete(int id, string userId)
        {
            string sql = "DELETE FROM admissions WHERE id = @id AND buyerId = @userId LIMIT 1;";
            _db.Execute(sql, new { id, userId });
        }




        internal IEnumerable<Admission> GetByProfileId(string id)
        {
            string sql = @"
      SELECT
      a.*,
      p.*
      FROM admissions a
      JOIN profiles p ON a.buyerId = p.id
      WHERE buyerId = @id;";
            return _db.Query<Admission, Profile, Admission>(sql, (admission, profile) =>
            {
                admission.Buyer = profile;
                return admission;
            }, new { id }, splitOn: "id");

        }

    }
}