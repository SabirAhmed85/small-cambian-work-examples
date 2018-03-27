const express = require('express');
const http = require('http');
const bodyParser = require('body-parser');
const cli = require('cli');
const config = require('./config');
// const reloadify = require('reloadify')(__dirname + '/portal');

var app = express();

app.use(bodyParser.json());
app.disable('etag');
// app.use(reloadify);

cli.parse({
    environment: ['e', 'Environment to use', 'string', 'dev'],
    server: ['s', 'Server to use', 'string', 'portal'],
});

function relay(environment, server, localRequest) {
    return new Promise((resolve, reject) => {
        var path = localRequest.originalUrl;

        if (!!server.redirectionPath){
            path = path.replace(server.path, server.redirectionPath);
        }

        console.log("Path:" + path);

        var options = {
            host: environment.hostname,
            port: server.port,
            method: localRequest.method,
            headers: localRequest.headers,
            // Bugfix path
            path: path
        };

        var req = http.request(options, (res) => {
            var str = "";

            res.on('data', (chunk) => {
                str += chunk;
            });

            res.on('end', () => {
                console.log(`Request: ${localRequest.method} ${localRequest.originalUrl}, Response: ${res.statusCode}`)
                resolve({ "data": str, "remoteResponse": res });
            });
        });

        req.on('error', (e) => {
            console.log(`Error: ${e}`)
            reject(e);
        })

        req.write(JSON.stringify(localRequest.body));
        req.end();
    })
}

function startServer(env, serv) {
    serv.contextPath = serv.contextPath || "/";

    app.use(serv.contextPath, express.static(__dirname + `/${serv.name}`));
    app.use(serv.path, function (req, res) {
        relay(env, serv, req).then((response) => {
            res.set(response.remoteResponse.headers).status(response.remoteResponse.statusCode).send(response.data);
        },
            (err) => {
                res.set(response.remoteResponse.headers).status(err.status).send(err.data);
            });
    });

    app.listen(3000);

};

cli.main(function (args, options) {
    console.log("Environment: " + options.environment);
    console.log("Server: " + options.server);

    config.forEach(function (env) {
        if (options.environment === env.serverName) {
            env.servers.forEach(function (serv) {
                if (options.server === serv.name) {
                    startServer(env, serv);
                }
            })
        }
    });
})

