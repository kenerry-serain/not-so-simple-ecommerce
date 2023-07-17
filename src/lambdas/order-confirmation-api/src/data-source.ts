import { DataSource } from "typeorm";
import { Order } from "./entities/order";
import { env } from "./env";

export const AppDataSource = new DataSource({
    type: "postgres",
    host: env.TYPEORM_HOST,
    port: env.TYPEORM_PORT,
    username: env.TYPEORM_USERNAME,
    password: env.TYPEORM_PASSWORD,
    database: env.TYPEORM_DATABASE,
    synchronize: false,
    logging: true,
    entities: [Order],
    subscribers: [],
    migrations: [],
});


