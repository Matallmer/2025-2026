import { PrismaClient } from "../prisma/client/client.ts";
import { fakerDE_AT } from "@faker-js/faker";

Deno.env.set("DATABASE_URL", "file:./dev.db");
const prisma = new PrismaClient();

async function main() {
  try {
    const newUser = await prisma.user.create({
      data: {
        email: fakerDE_AT.internet.email(),
        name: fakerDE_AT.person.firstName(),
      },
    });

    console.log("Neuer Benutzer hinzugefügt:", newUser);
  } catch (e) {
    console.error("Fehler beim Hinzufügen des Benutzers:", e);
    Deno.exit(1);
  } finally {
    await prisma.$disconnect();
  }
}

main();
