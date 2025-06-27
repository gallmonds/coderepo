export interface CommentAlgorithmDto {
  contentId: number;
  body: string;
  replyToId?: number | null;
}