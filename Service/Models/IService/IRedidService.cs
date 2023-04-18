using Model.Models;

namespace Service.Models.IService
{
    public interface IRedidService : IBaseService<RedisData>
    {
        public RedisData GetRedisDataByKey(string key);
        public RedisData SetRedisDataByKey(RedisData redisData);
    }
}
