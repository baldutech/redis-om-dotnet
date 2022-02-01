﻿using Redis.OM.Aggregation;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Redis.OM
{
    /// <summary>
    /// Provides a connection to redis.
    /// </summary>
    public class RedisConnectionProvider
    {
        private readonly IConnectionMultiplexer _mux;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The string to use to connect to redis.</param>
        public RedisConnectionProvider(string connectionString)
        {
            var options = RedisUriParser.ParseConfigFromUri(connectionString);
            _mux = ConnectionMultiplexer.Connect(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionConfig">The configuration.</param>
        public RedisConnectionProvider(RedisConnectionConfiguration connectionConfig)
        {
            _mux = ConnectionMultiplexer.Connect(connectionConfig.ToStackExchangeConnectionString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionProvider"/> class.
        /// </summary>
        /// <param name="configurationOptions">The options relevant to a set of redis connections.</param>
        public RedisConnectionProvider(ConfigurationOptions configurationOptions)
        {
            _mux = ConnectionMultiplexer.Connect(configurationOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionProvider"/> class.
        /// </summary>
        /// <param name="connectionMultiplexer">The options relevant to a set of redis connections.</param>
        public RedisConnectionProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            _mux = connectionMultiplexer;
        }

        /// <summary>
        /// Gets a command level interface to redis.
        /// </summary>
        public IRedisConnection Connection => new RedisConnection(_mux.GetDatabase());

        /// <summary>
        /// Gets an aggregation set for redis.
        /// </summary>
        /// <typeparam name="T">The indexed type to run aggregations on.</typeparam>
        /// <returns>the aggregation set.</returns>
        public RedisAggregationSet<T> AggregationSet<T>() => new (Connection);

        /// <summary>
        /// Gets a redis collection.
        /// </summary>
        /// <typeparam name="T">The type the collection will be retrieving.</typeparam>
        /// <returns>A RedisCollection.</returns>
        public IRedisCollection<T> RedisCollection<T>()
            where T : notnull => new RedisCollection<T>(Connection);
    }
}
