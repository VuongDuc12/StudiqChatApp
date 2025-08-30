import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

let connection: HubConnection | null = null;

export function getConnection(token: string): HubConnection {
  if (connection && connection.state !== "Disconnected") {
    return connection;
  }
  connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5210/hubs/chat", {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  return connection;
}

export async function ensureConnected(token: string): Promise<HubConnection> {
  const conn = getConnection(token);
  if (conn.state === 'Connected') return conn;
  try {
    await conn.start();
    // eslint-disable-next-line no-console
    console.info('SignalR connected', conn.connectionId);
  } catch (err) {
    // eslint-disable-next-line no-console
    console.warn('SignalR start failed', err);
    throw err;
  }
  return conn;
}

export function stopConnection() {
  if (connection) {
    connection.stop();
    connection = null;
  }
}
