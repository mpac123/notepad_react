export default class ApiService {
  constructor(cfg) {
    this.serverUrl = cfg.serverUrl;
    console.log("backend: ", this.serverUrl);
  }
  async sendRequest(url, method, data = null) {
    const errorTitle = `Error performing ${method} ${url} `;
    let fetchBody = {
      method: method,
      headers: {
        "Content-Type": "application/json; charset=utf-8"
      }
    };
    if (data !== null) {
      fetchBody = { ...fetchBody, body: JSON.stringify(data) };
    }
    let fetchResponse = await fetch(url, fetchBody);
    if (fetchResponse.ok) {
      try {
        return await fetchResponse.json();
      } catch (err) {
        console.error(err);
        // throw Object.create(err);
      }
    } else {
      let resp = await fetchResponse.statusText;
      try {
        resp = await fetchResponse.json();
      } catch (err) {
        console.error(resp, err);
      }
      /*Using statusText instead since if it'se.g. a 404, the .json() explodes*/
      throw Object.create({ title: errorTitle, message: resp });
    }
  }

  async getNote(id) {
    return this.sendRequest(`${this.serverUrl}/notes/${id}`, "GET");
  }
  async getNotes(
    page = undefined,
    dateFrom = undefined,
    dateTo = undefined,
    category = undefined
  ) {
    let query = "?";
    if (page) {
      query += `page=${page}&`;
    }
    if (dateFrom) {
      query += `dateFrom=${dateFrom}&dateTo=${dateTo}&`;
    }
    if (category) {
      query += `category=${category}&`;
    }
    query = query.slice(0, -1);
    return this.sendRequest(`${this.serverUrl}/notes${query}`, "GET");
  }

  async getCategories() {
    return this.sendRequest(`${this.serverUrl}/notes/categories`, "GET");
  }

  async addNote(body) {
    return this.sendRequest(`${this.serverUrl}/notes`, "POST", body);
  }

  async updateNote(title, body) {
    return this.sendRequest(`${this.serverUrl}/notes/${title}`, "POST", body);
  }

  async deleteNote(title) {
    return this.sendRequest(`${this.serverUrl}/notes/${title}`, "DELETE");
  }
}
