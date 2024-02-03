# multi tms api
a simple poc for loading different interfaces based on a client configuration that is stored in a database.

## goals
create a single interface that will only be accessed via a provider that reads the clients configured plugin id.
plugins may or may not have matching features. handle that validation.
clients may have a plugin that is capable of a feature but no permission to use the feature.
clients to use simple api key authorization.
a shipment should never be stored in the "cache" db unless it the plugin method runs successfully.

### Requires
MongoDB to be hosted in Docker.