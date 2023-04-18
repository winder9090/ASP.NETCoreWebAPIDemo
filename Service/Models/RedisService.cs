using Common.Cache;
using Infrastructure.Attribute;
using Model.Models;
using Service.Models.IService;

namespace Service.Models
{
    [AppService(ServiceType = typeof(IRedidService), ServiceLifetime = LifeTime.Transient)]
    public class RedisService : BaseService<RedisData>, IRedidService
    {
        public RedisData GetRedisDataByKey(string key)
        {
            RedisData redisData = new RedisData();
            redisData.Status = false;
            if (RedisServer.Cache.Get(key) is string value)
            {
                redisData.RedisKey = key;
                redisData.RedisValue = value;
                redisData.Status = true;
            }
            return redisData;

        }

        public RedisData SetRedisDataByKey(RedisData redisData)
        {
            redisData.Status = RedisServer.Cache.Set(redisData.RedisKey, redisData.RedisValue);
            return redisData;
        }


    }
}
