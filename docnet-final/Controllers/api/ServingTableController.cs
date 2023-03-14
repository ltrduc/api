using docnet_final.Context;
using docnet_final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace docnet_final.Controllers.api
{
    public class ServingTableController : ApiController
    {
        DocNetFinalDataBase db = new DocNetFinalDataBase();

        // GET: api/ServingTable
        [Route("api/serving-table")]
        [HttpGet]
        public Wrapper<IEnumerable<ServingTable>> Get()
        {
            var servingTables = db.ServingTables.Where(t => t.Active == 1).OrderByDescending(t => t.ServingTableID).ToList();

            var result = new Wrapper<IEnumerable<ServingTable>> 
            {
                Success = servingTables.Any(),
                Message = servingTables.Any() ? "" : "Không tìm thấy dữ liệu!",
                Data = servingTables
            };

            return result;
        }

        // GET: api/ServingTable/5
        [Route("api/serving-table/{id}")]
        [HttpGet]
        public Wrapper<ServingTable> Get(int id)
        {
            var servingTable = db.ServingTables.FirstOrDefault(t => t.Active == 1 && t.ServingTableID == id);

            var result = new Wrapper<ServingTable>
            {
                Success = servingTable != null,
                Message = servingTable != null ? "" : "Không tìm thấy dữ liệu!",
                Data = servingTable
            };

            return result;
        }

        // POST: api/ServingTable
        [Route("api/serving-table")]
        [HttpPost]
        public Wrapper<ServingTable> Post([FromBody] ServingTable servingTable)
        {
            var result = new Wrapper<ServingTable>();

            if (string.IsNullOrEmpty(servingTable.ServingTableCode))
            {
                result.Success = false;
                result.Message = "Mã bàn không được để trống!";
                return result;
            }

            if (servingTable.Capacity <= 0)
            {
                result.Success = false;
                result.Message = "Sức chứa phải lớn hơn 0!";
                return result;
            }

            if (db.ServingTables.Any(t => t.ServingTableCode == servingTable.ServingTableCode && t.Active == 1))
            {
                result.Success = false;
                result.Message = "Mã bàn đã tồn tại trong hệ thống!";
                return result;
            }

            try
            {
                servingTable.Active = 1;
                servingTable.CreatedAt = DateTime.Now;
                servingTable.UpdatedAt = DateTime.Now;
                db.ServingTables.Add(servingTable);
                db.SaveChanges();

                result.Success = true;
                result.Message = "Thêm dữ liệu thành công!";
                result.Data = servingTable;
            }
            catch (Exception ex)
            {
                result.Success = true;
                result.Message = "Thêm dữ liệu không thành công! Lỗi: " + ex.Message;
                result.Data = null;
            }

            return result;
        }

        // PUT: api/ServingTable/5
        [Route("api/serving-table/{id}")]
        [HttpPut]
        public Wrapper<ServingTable> Put(int id, [FromBody] ServingTable servingTable)
        {
            var result = new Wrapper<ServingTable>();

            var existingTable = db.ServingTables.FirstOrDefault(t => t.Active == 1 && t.ServingTableID == id);

            if (existingTable == null)
            {
                result.Success = false;
                result.Message = "Không tìm thấy dữ liệu!";
                return result;
            }

            if (string.IsNullOrEmpty(servingTable.ServingTableCode))
            {
                result.Success = false;
                result.Message = "Mã bàn không được để trống!";
                return result;
            }

            if (servingTable.Capacity <= 0)
            {
                result.Success = false;
                result.Message = "Sức chứa phải lớn hơn 0!";
                return result;
            }

            if (db.ServingTables.Any(t => t.ServingTableCode == servingTable.ServingTableCode && t.Active == 1 && t.ServingTableID != id))
            {
                result.Success = false;
                result.Message = "Mã bàn đã tồn tại trong hệ thống!";
                return result;
            }

            try
            {
                existingTable.ServingTableCode = servingTable.ServingTableCode;
                existingTable.Capacity = servingTable.Capacity;
                existingTable.Description = servingTable.Description;
                existingTable.Status = servingTable.Status;
                existingTable.UpdatedAt = DateTime.Now;

                db.SaveChanges();

                result.Success = true;
                result.Message = "Cập nhật dữ liệu thành công!";
                result.Data = existingTable;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Cập nhật dữ liệu không thành công! Lỗi: " + ex.Message;
                result.Data = existingTable;
            }

            return result;
        }

        // DELETE: api/ServingTable/5
        [Route("api/serving-table/{id}")]
        [HttpDelete]
        public Wrapper<ServingTable> Delete(int id)
        {
            var result = new Wrapper<ServingTable>();
            var servingTable = db.ServingTables.FirstOrDefault(t => t.Active == 1 && t.ServingTableID == id);

            if (servingTable == null)
            {
                result.Success = false;
                result.Message = "Không tìm thấy dữ liệu!";
                return result;
            }

            try
            {
                servingTable.Active = 0;
                db.SaveChanges();

                result.Success = true;
                result.Message = "Xóa dữ liệu thành công!";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Xóa dữ liệu không thành công! Lỗi: " + ex.Message;
                return result;
            }
        }
    }
}
