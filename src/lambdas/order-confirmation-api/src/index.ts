import * as AWS from 'aws-sdk';
import { Client } from 'pg';
import { Context } from 'aws-lambda';

const secretsManager = new AWS.SecretsManager();
const sns = new AWS.SNS();
const rdsProxyEndpoint = process.env.RDS_PROXY_ENDPOINT as string;
const secretArn = process.env.RDS_SECRET_ARN as string;
const snsTopicArn = process.env.SNS_TOPIC_ARN as string;
const databaseName = process.env.RDS_DATABASE_NAME as string;

export const handler = async (event: any, _: Context) => {

    console.log(event);

    /* Recuperando Secret do Banco de Dados */
    const secret = await getSecret();
    const client = new Client({
        host: rdsProxyEndpoint,
        user: secret.username,
        password: secret.password,
        database: databaseName,
        ssl: true,
        port: 5432
    });
    
    const body = typeof event.body === 'string' ? JSON.parse(event.body) : event.body;
    if (!body || !body.Id) {
        throw new Error('Id is missing from the event body');
    }

    /* Conectando no Banco de Dados */
    await client.connect();

    try {
        /* Atualizando Status da Ordem para Confirmado */
        const sql = `UPDATE "Order" SET "StatusId" = 1 WHERE "Id" = $1`;
        const values = [body.Id];
        const response = await client.query(sql, values);
        console.log('Order updated successfully:', response);

        /* Publicando mensagem no tópico orderConfirmed */
        await publishToSns(JSON.stringify({Id: body.Id}));
        return response;
    } catch (error) {
        console.error('Failed to update Order:', error);
        throw error;
    } finally {
        await client.end();
    }
};

async function getSecret(): Promise<any> {
    const data = await secretsManager.getSecretValue({ SecretId: secretArn }).promise();
    if ('SecretString' in data) {
        return JSON.parse(data.SecretString as string);
    }
    throw new Error('Secret not found');
}

async function publishToSns(message: string): Promise<void> {
    const params = {
        Message: message,
        TopicArn: snsTopicArn,
    };
    await sns.publish(params).promise();
    console.log('Message published to SNS topic:', message);
}
